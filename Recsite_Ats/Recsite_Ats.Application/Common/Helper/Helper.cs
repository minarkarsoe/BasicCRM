using System.Drawing.Imaging;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using static Recsite_Ats.Domain.Extend.AdminSetting;
using Image = System.Drawing.Image;

namespace Recsite_Ats.Application.Common.Helpers;

public static class Helper
{
    public static readonly Dictionary<Setting, string> UnwantedColumn = new Dictionary<Setting, string>
    {
        { Setting.Companies , "Id,CreatedDate,CreatedBy,EditedDate,EditedBy,AccountId" },
        { Setting.Jobs , "Id,CreatedDate,CreatedBy,EditedDate,EditedBy,AccountId" },
        { Setting.Candidates , "Id,CreatedDate,CreatedBy,EditedDate,EditedBy,AccountId" },
        { Setting.Contacts , "Id,CreatedDate,CreatedBy,EditedDate,EditedBy,AccountId,CompanyId"}
    };
    public static string GetColumnList(this Setting setting)
    {
        return UnwantedColumn[setting];
    }

    private static readonly string AuthenticatorUriFormat =
       "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";


    public static byte[] GetOneMbImage(string base64ImageData)
    {
        byte[] imageData = Convert.FromBase64String(base64ImageData);

        string imageName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_image.jpg";
        string fileName = @"C:\imagecreate\" + imageName;

        using (MemoryStream ms = new MemoryStream(imageData))
        {
            using (Image image = Image.FromStream(ms))
            {
                double mb = (imageData.Length / 1024f) / 1024f;

                if (mb >= 1f)
                {
                    long maxSize = 1 * 1024 * 1024;
                    SaveJpgAtFileSize(image, fileName, maxSize);

                    using (var reducedMS = new MemoryStream())
                    {
                        using (Image myImage = Image.FromFile(fileName, true))
                        {
                            myImage.Save(reducedMS, myImage.RawFormat);
                            imageData = reducedMS.ToArray();
                        }
                    }
                }
            }
        }

        File.Delete(fileName);

        return imageData;
    }


    public static int SaveJpgAtFileSize(Image image, string file_name, long max_size)
    {
        for (int level = 100; level > 5; level -= 5)
        {
            // Try saving at this compression level.                
            SaveJpeg(image, file_name, level);

            // If the file is small enough, we're done.
            long fileSize = GetFileSize(file_name);
            if (GetFileSize(file_name) <= max_size) return level;
        }

        // Stay with level 5.
        return 5;
    }

    public static void SaveJpeg(Image image, string file_name, long compression)
    {

        EncoderParameters encoder_params = new EncoderParameters(1);
        encoder_params.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, compression);

        ImageCodecInfo image_codec_info = GetEncoderInfo("image/jpeg");
        image.Save(file_name, image_codec_info, encoder_params);
    }

    public static long GetFileSize(string file_name)
    {
        return new FileInfo(file_name).Length;
    }

    public static string CreateHardCodeSecretKey()
    {
        return "asdefZXCVB~!@#$%";
    }

    public static string CreateHardCodeIV()
    {
        return "111ASD!@#$%^&*33";
    }


    public static string EncryptAES(string text, string inputKey, string IV)
    {

        var md5 = new MD5CryptoServiceProvider();
        var Passcode = md5.ComputeHash(Encoding.UTF8.GetBytes(inputKey));
        var IVBytes = Encoding.UTF8.GetBytes(IV);

        string iv = Convert.ToBase64String(IVBytes);

        //Initialize objects
        var cipher = new RijndaelManaged();
        var encryptor = cipher.CreateEncryptor(Passcode, IVBytes);

        try
        {
            var buffer = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(encryptor.TransformFinalBlock(buffer, 0, buffer.Length));
        }
        catch (ArgumentException ae)
        {
            return null;
        }
        catch (ObjectDisposedException oe)
        {
            return null;
        }
    }

    public static string DecryptAES(string text, string inputKey, string IV)
    {
        var md5 = new MD5CryptoServiceProvider();
        var Passcode = md5.ComputeHash(Encoding.UTF8.GetBytes(inputKey));
        var IVBytes = Encoding.UTF8.GetBytes(IV);

        //Initialize objects
        var cipher = new RijndaelManaged();
        var decryptor = cipher.CreateDecryptor(Passcode, IVBytes);

        try
        {
            byte[] input = Convert.FromBase64String(text);

            var newClearData = decryptor.TransformFinalBlock(input, 0, input.Length);
            return Encoding.UTF8.GetString(newClearData);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public static string GenerateQrCodeUri(string email, string unformattedKey)
    {
        return string.Format(
        AuthenticatorUriFormat,
            UrlEncoder.Default.Encode("Recsite Recruit CRM"),
            UrlEncoder.Default.Encode(email),
            unformattedKey);
    }


    private static ImageCodecInfo GetEncoderInfo(string mimeType)
    {
        // Get image codecs for all image formats 
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

        // Find the correct image codec 
        for (int i = 0; i < codecs.Length; i++)
            if (codecs[i].MimeType == mimeType)
                return codecs[i];

        return null;
    }


    public static bool IsNumber(string value)
    {
        if (string.IsNullOrEmpty(value)) return false;
        return int.TryParse(value, out _);
    }

    public static bool IsDateTime(string value)
    {
        if (string.IsNullOrEmpty(value)) return false;
        return DateTime.TryParse(value, out _);
    }

    public static bool IsBoolean(string value)
    {
        if (string.IsNullOrEmpty(value)) { return false; }
        return bool.TryParse(value, out _);
    }
}
