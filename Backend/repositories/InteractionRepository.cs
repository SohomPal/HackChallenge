using System.Data;
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
        const string query = "SELECT * FROM interactions";
        return _db.Query<Interaction>(query);
    }

    public Interaction GetById(int id)
    {
        const string query = "SELECT * FROM interactions WHERE id = @Id";
        return _db.QueryFirstOrDefault<Interaction>(query, new { Id = id });
    }

    public void Create(Interaction interaction)
    {
        const string query = @"INSERT INTO interactions (lead_id, type, summary, sentiment, transcript, created_at) 
                                VALUES (@LeadId, @Type, @Summary, @Sentiment, @Transcript, @CreatedAt)";
        _db.Execute(query, interaction);
    }

    public void Update(Interaction interaction)
    {
        const string query = @"UPDATE interactions 
                                SET type = @Type, summary = @Summary, sentiment = @Sentiment, transcript = @Transcript
                                WHERE id = @Id";
        _db.Execute(query, interaction);
    }

    public void Delete(int id)
    {
        const string query = "DELETE FROM interactions WHERE id = @Id";
        _db.Execute(query, new { Id = id });
    }

    // ✅ new — used by RecommendNextActionsHandler
    public async Task<IEnumerable<Interaction>> GetRecentByLeadId(int leadId, int limit = 10)
    {
        const string query = @"SELECT * FROM interactions 
                               WHERE lead_id = @LeadId 
                               ORDER BY created_at DESC";
        return await _db.QueryAsync<Interaction>(query, new { LeadId = leadId, Limit = limit });
    }
}