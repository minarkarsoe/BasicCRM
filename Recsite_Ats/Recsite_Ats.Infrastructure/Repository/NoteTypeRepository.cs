﻿using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Domain.Entites;
using Recsite_Ats.Infrastructure.Data;

namespace Recsite_Ats.Infrastructure.Repository;
public class NoteTypeRepository : Repository<NoteType>, INoteTypeRepository
{
    private readonly ApplicationDbContext _db;
    public NoteTypeRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
}
