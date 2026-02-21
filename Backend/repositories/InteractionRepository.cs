using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Backend.Models;
using Dapper;

namespace Backend.Repositories;
public class InteractionRepository
{
    private readonly IDbConnection _db;

    public InteractionRepository(IDbConnection db)
    {
        _db = db;
    }

    public IEnumerable<Interaction> GetAll()
    {
        const string query = "SELECT * FROM Interactions";
        return _db.Query<Interaction>(query);
    }

    public Interaction GetById(int id)
    {
        const string query = "SELECT * FROM Interactions WHERE Id = @Id";
        return _db.QueryFirstOrDefault<Interaction>(query, new { Id = id });
    }

    public void Create(Interaction interaction)
    {
        const string query = @"INSERT INTO Interactions (Name, Description, CreatedDate) 
                                VALUES (@Name, @Description, @CreatedDate)";
        _db.Execute(query, interaction);
    }

    public void Update(Interaction interaction)
    {
        const string query = @"UPDATE Interactions 
                                SET Name = @Name, Description = @Description 
                                WHERE Id = @Id";
        _db.Execute(query, interaction);
    }

    public void Delete(int id)
    {
        const string query = "DELETE FROM Interactions WHERE Id = @Id";
        _db.Execute(query, new { Id = id });
    }
}
