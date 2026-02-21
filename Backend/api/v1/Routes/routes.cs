using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Npgsql;
using Microsoft.AspNetCore.Http;

namespace api.v1.Routes;

public static class Routes
{
    public static void MapRoutes(this IEndpointRouteBuilder app){
        RouteGroupBuilder todoRoutes = app.MapGroup("/api/v1/backend");

        // AI “actions” (research, drafts, analysis)
        app.MapAiRoutes();

        // External integrations (Calendly/Twilio webhooks, voice reminders)
        app.MapIntegrationRoutes();
    }

    // ----------------------------
    // LEADS (CRUD + search + scoring)
    // ----------------------------
   

    


    

    // ----------------------------
    // AI ROUTES (research + drafting + analysis)
    // ----------------------------
    public static void MapAiRoutes(this IEndpointRouteBuilder app)
    {
        var ai = app.MapGroup("/api/v1/ai");

        // Lead discovery / research
        ai.MapPost("/research/lead", ResearchLeadHandler);           // input: company/linkedin/url -> output: research summary + fit score
        ai.MapPost("/discover/leads", DiscoverLeadsHandler);         // input: ICP -> output: list of leads (optional)

        // Outreach drafting
        ai.MapPost("/draft/cold-email", DraftColdEmailHandler);      // input: leadId + context -> output: subject/body
        ai.MapPost("/draft/linkedin", DraftLinkedInMessageHandler);  // input: leadId -> output: DM text
        ai.MapPost("/draft/follow-up", DraftFollowUpHandler);        // input: leadId + last interaction -> output follow-up

        // Call / meeting analysis
        ai.MapPost("/analyze/transcript", AnalyzeTranscriptHandler); // input: interactionId/transcript -> output: sentiment, objections, next steps
        ai.MapPost("/estimate/deal-value", EstimateDealValueHandler);// input: leadId + transcript/history -> output: value_estimate + probability

        // Recommendations
        ai.MapGet("/recommend/next-actions/{leadId:guid}", RecommendNextActionsHandler);
    }

    // ----------------------------
    // INTEGRATIONS (Calendly, Twilio, ElevenLabs triggers)
    // ----------------------------
    public static void MapIntegrationRoutes(this IEndpointRouteBuilder app)
    {
        var integrations = app.MapGroup("/api/v1/integrations");
    
        // Calendly webhooks: meeting booked/cancelled
        integrations.MapPost("/calendly/webhook", CalendlyWebhookHandler);

        // Twilio: call status + recording callback (store recording URL, create interaction, kick off transcription)
        integrations.MapPost("/twilio/voice/status", TwilioVoiceStatusWebhookHandler);
        integrations.MapPost("/twilio/voice/recording", TwilioRecordingWebhookHandler);

        // Trigger outbound reminders/calls (your app calls Twilio + ElevenLabs)
        integrations.MapPost("/voice/reminder/{leadId:guid}", TriggerReminderCallHandler); // input: meeting/time/message
        integrations.MapPost("/voice/voicemail-drop/{leadId:guid}", TriggerVoicemailDropHandler);

        // Email provider webhook (optional)
        integrations.MapPost("/email/webhook", EmailWebhookHandler); // open/click/bounce -> update interaction/deal signals
    }

    // ============================================================
    // HANDLER PLACEHOLDERS (add your actual implementations)
    // ============================================================   

    // Leads
    private static async Task<IResult> GetLeadsHandler(){

    var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false)
        .Build();

    var connString = config.GetConnectionString("Db");

    const string sql = "SELECT * FROM leads";

    await using var conn = new NpgsqlConnection(connString);
    await conn.OpenAsync();

    await using var cmd = new NpgsqlCommand(sql, conn);
    await using var reader = await cmd.ExecuteReaderAsync();

    var results = new List<Dictionary<string, object?>>();

    while (await reader.ReadAsync())
    {
        var row = new Dictionary<string, object?>();

        for (int i = 0; i < reader.FieldCount; i++)
        {
            row[reader.GetName(i)] =
                await reader.IsDBNullAsync(i) ? null : reader.GetValue(i);
        }

        results.Add(row);
    }

    return Results.Ok(results);
}

    private static IResult GetLeadByIdHandler(Guid id) => Results.NotImplemented();
    private static IResult GetLeadTimelineHandler(Guid id) => Results.NotImplemented();
    private static IResult SearchLeadsHandler(string q) => Results.NotImplemented();
    private static IResult CreateLeadHandler() => Results.NotImplemented();
    private static IResult BulkCreateLeadsHandler() => Results.NotImplemented();
    private static IResult UpdateLeadHandler(Guid id) => Results.NotImplemented();
    private static IResult UpdateLeadFitScoreHandler(Guid id) => Results.NotImplemented();
    private static IResult DeleteLeadHandler(Guid id) => Results.NotImplemented();

    // Deals
    private static IResult GetDealsHandler() => Results.NotImplemented();
    private static IResult GetDealByIdHandler(Guid id) => Results.NotImplemented();
    private static IResult GetDealsByLeadHandler(Guid leadId) => Results.NotImplemented();
    private static IResult CreateDealHandler() => Results.NotImplemented();
    private static IResult UpdateDealHandler(Guid id) => Results.NotImplemented();
    private static IResult UpdateDealStageHandler(Guid id) => Results.NotImplemented();
    private static IResult UpdateNextActionDateHandler(Guid id) => Results.NotImplemented();
    private static IResult DeleteDealHandler(Guid id) => Results.NotImplemented();

    // Interactions
    private static IResult GetInteractionsHandler()
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult GetInteractionByIdHandler(Guid id)
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult GetInteractionsByLeadHandler(Guid leadId)
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult CreateInteractionHandler()
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult AttachTranscriptHandler(Guid id)
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult UpdateInteractionHandler(Guid id)
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult DeleteInteractionHandler(Guid id)
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    // Tasks
    private static IResult GetTasksHandler()
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult GetTaskByIdHandler(Guid id)
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult GetTasksByLeadHandler(Guid leadId)
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult GetTasksDueTodayHandler()
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult GetOverdueTasksHandler()
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult CreateTaskHandler()
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult ScheduleAfterDemoFollowUpsHandler(Guid leadId)
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult UpdateTaskHandler(Guid id)
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult MarkTaskCompleteHandler(Guid id)
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult ReopenTaskHandler(Guid id)
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult DeleteTaskHandler(Guid id)
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    // AI
    private static IResult ResearchLeadHandler()
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult DiscoverLeadsHandler()
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult DraftColdEmailHandler()
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult DraftLinkedInMessageHandler()
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult DraftFollowUpHandler()
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult AnalyzeTranscriptHandler()
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult EstimateDealValueHandler()
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult RecommendNextActionsHandler(Guid leadId)
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    // Integrations
    private static IResult CalendlyWebhookHandler()
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult TwilioVoiceStatusWebhookHandler()
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult TwilioRecordingWebhookHandler()
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult TriggerReminderCallHandler(Guid leadId)
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult TriggerVoicemailDropHandler(Guid leadId)
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult EmailWebhookHandler()
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }
}