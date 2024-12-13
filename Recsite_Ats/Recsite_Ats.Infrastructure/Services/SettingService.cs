using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Recsite_Ats.Application.Common.Helpers;
using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Application.Common.Interface.Services;
using Recsite_Ats.Domain.API.APIRequest;
using Recsite_Ats.Domain.API.APIResponse;
using Recsite_Ats.Domain.DataTransferObject;
using Recsite_Ats.Domain.DataTransferObject.UserDTO;
using Recsite_Ats.Domain.Entites;
using System.Data;

namespace Recsite_Ats.Infrastructure.Services;
public class SettingService : ISettingService
{
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;
    public SettingService(IConfiguration configuration, IUnitOfWork unitOfWork)
    {
        _configuration = configuration;
        _unitOfWork = unitOfWork;
    }

    public async Task<SectionLayoutDTO> GetFieldDetailsAsync(string tableName, int? accountId, string columnNames)
    {

        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        var results = new List<CustomFieldsDTO>();
        var sectionList = new List<SectionDTO>();
        var sectionLayOutList = new SectionLayoutDTO();
        string storedProcedureName = "GetDefaultFieldList";
        var parameters = new[]
        {
            new SqlParameter("@TableName", SqlDbType.NVarChar) { Value = (object)tableName ?? DBNull.Value },
            new SqlParameter("@AccountId" , SqlDbType.Int) { Value = (object)accountId ?? DBNull.Value },
            new SqlParameter("@ColumnNames" , SqlDbType.NVarChar) { Value = (object)columnNames ?? DBNull.Value },
            // Add more parameters as needed
        };

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddRange(parameters);

                try
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        int i = 0;
                        while (reader.Read())
                        {
                            var customField = new CustomFieldsDTO
                            {
                                Index = i++,
                                FieldName = reader.GetString(reader.GetOrdinal("FieldName")),
                                CustomFieldTypeId = reader.IsDBNull(reader.GetOrdinal("CustomFieldTypeId")) ? default : reader.GetInt32(reader.GetOrdinal("CustomFieldTypeId")),
                                CustomFieldTypeName = reader.GetString(reader.GetOrdinal("CustomFieldTypeName")),
                                SectionLayoutId = reader.IsDBNull(reader.GetOrdinal("SectionLayoutId")) ? default : reader.GetInt32(reader.GetOrdinal("SectionLayoutId")),
                                IsNullable = reader.IsDBNull(reader.GetOrdinal("IsNullable")) ? default : reader.GetBoolean(reader.GetOrdinal("IsNullable")),
                                IsLocked = reader.IsDBNull(reader.GetOrdinal("IsLocked")) ? default : reader.GetBoolean(reader.GetOrdinal("IsLocked")),
                                IsRequired = reader.IsDBNull(reader.GetOrdinal("IsRequired")) ? default : reader.GetBoolean(reader.GetOrdinal("IsRequired")),
                                IsVisible = reader.IsDBNull(reader.GetOrdinal("IsVisible")) ? default : reader.GetBoolean(reader.GetOrdinal("IsVisible")),
                                SortOrder = reader.IsDBNull(reader.GetOrdinal("SortOrder")) ? default : reader.GetInt64(reader.GetOrdinal("SortOrder")),
                                IsCustomField = reader.IsDBNull(reader.GetOrdinal("IsCustomField")) ? default : reader.GetBoolean(reader.GetOrdinal("IsCustomField")),
                                TableName = reader.IsDBNull(reader.GetOrdinal("TableName")) ? null : reader.GetString(reader.GetOrdinal("TableName")),
                                AccountId = reader.IsDBNull(reader.GetOrdinal("AccountId")) ? default : reader.GetInt32(reader.GetOrdinal("AccountId")),
                                Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? default : reader.GetInt32(reader.GetOrdinal("Id"))
                            };

                            results.Add(customField);
                        }
                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                var section = new SectionDTO
                                {
                                    SectionLayoutId = reader.GetInt32(reader.GetOrdinal("SectionLayoutId")),
                                    SectionName = reader.GetString(reader.GetOrdinal("SectionName")),
                                    TableName = reader.GetString(reader.GetOrdinal("TableName")),
                                    Sort = reader.GetInt32(reader.GetOrdinal("Sort")),
                                    Visible = reader.GetBoolean(reader.GetOrdinal("Visible")),
                                    AccountId = reader.IsDBNull(reader.GetOrdinal("AccountId")) ? null : reader.GetInt32(reader.GetOrdinal("AccountId")),
                                    IsCustomSection = reader.GetBoolean(reader.GetOrdinal("IsCustomSection"))
                                };
                                sectionList.Add(section);
                            }
                        }

                        sectionLayOutList.Sections = sectionList;
                        sectionLayOutList.CustomFields = results;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }

        return sectionLayOutList;
    }

    public async Task<FieldLayout?> CreateFieldLayout(FieldLayout fieldLayout)
    {
        var test = await _unitOfWork.FieldLayout.Add(fieldLayout);
        await _unitOfWork.Save();
        return test;
    }
    public async Task<List<CountryDataResponse>> GetCountryData()
    {
        var countryList = new List<CountryDataResponse>();
        var getcountryData = await _unitOfWork.CountryData.GetAll();

        foreach (var country in getcountryData)
        {
            var countryData = new CountryDataResponse()
            {
                ISO_Code = country.ISO_CODES,
                Name = country.CountryName
            };
            countryList.Add(countryData);
        }
        return countryList;
    }

    public async Task AddBillingInformation(BillInformationDTO request, ClaimTypesDto claimTypes)
    {
        var checkBillingInfo = await _unitOfWork.BillingInformation.Get(x => x.AccountId == claimTypes.AccountId);

        if (checkBillingInfo != null)
        {
            checkBillingInfo.AccountId = claimTypes.AccountId;
            checkBillingInfo.CompanyName = request.CompanyName;
            checkBillingInfo.AddressLine1 = request.AddressLine1;
            checkBillingInfo.AddressLine2 = request.AddressLine2;
            checkBillingInfo.City = request.City;
            checkBillingInfo.Country = request.Country.Name;
            checkBillingInfo.ZipCode = request.ZipCode;
            checkBillingInfo.VATNumber = request.VATNumber;
            checkBillingInfo.CreatedAt = DateTime.Now;
            checkBillingInfo.UpdatedAt = DateTime.Now;
            checkBillingInfo.CreatedBy = claimTypes.UserId;
            await _unitOfWork.BillingInformation.Update(checkBillingInfo);
        }
        else
        {
            BillingInformation billingInformation = new BillingInformation()
            {
                AccountId = claimTypes.AccountId,
                CompanyName = request.CompanyName,
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                City = request.City,
                Country = request.Country.Name,
                ZipCode = request.ZipCode,
                VATNumber = request.VATNumber,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = claimTypes.UserId,
            };
            await _unitOfWork.BillingInformation.Add(billingInformation);
        }

        await _unitOfWork.Save();
    }

    public async Task<BillInformationDTO> GetBillInformation(ClaimTypesDto claimTypes)
    {
        var billingInfo = await _unitOfWork.BillingInformation.Get(x => x.AccountId == claimTypes.AccountId);
        if (billingInfo != null)
        {
            var getcountry = await _unitOfWork.CountryData.Get(x => x.CountryName == billingInfo.Country);
            var response = new BillInformationDTO
            {
                CompanyName = billingInfo.CompanyName,
                AddressLine1 = billingInfo.AddressLine1,
                AddressLine2 = billingInfo.AddressLine2,
                City = billingInfo.City,
                Country = new Country() { ISO_Code = getcountry.ISO_CODES, Name = getcountry.CountryName },
                ZipCode = billingInfo.ZipCode,
                VATNumber = billingInfo.VATNumber,
            };
            return response;
        }
        return null;

    }

    public async Task AddPaymentMethod(PaymentMethodDTO request, ClaimTypesDto? claimTypes)
    {
        string hardCodeSecret = Helper.CreateHardCodeSecretKey();
        string hardCodeIV = Helper.CreateHardCodeIV();

        request.SecurityCode = Helper.EncryptAES(request.SecurityCode, hardCodeIV, hardCodeSecret);
        PaymentMethod paymentMethod = new PaymentMethod()
        {
            AccountId = claimTypes.AccountId,
            Name = request.Name,
            CardNumber = request.CardNumber,
            SecurityCode = request.SecurityCode,
            ExpiryMonth = request.ExpiryMonth,
            ExpiryYear = request.ExpiryYear,
        };
        await _unitOfWork.PaymentMethod.Add(paymentMethod);
        await _unitOfWork.Save();
    }
}
