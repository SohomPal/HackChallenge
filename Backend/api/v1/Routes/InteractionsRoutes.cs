

namespace api.v1.Routes;

public static class InteractionsRoutes
{

// ----------------------------
    // INTERACTIONS (email/call/meeting logging + transcripts)
    // ----------------------------
    public static void MapInteractionRoutes(this IEndpointRouteBuilder app)
    {
        var interactions = app.MapGroup("/api/v1/interactions");

        // GET
        interactions.MapGet("", GetInteractionsHandler);                                // list (filter by leadId, type, date)
        interactions.MapGet("/{id:guid}", GetInteractionByIdHandler);
        interactions.MapGet("/by-lead/{leadId:guid}", GetInteractionsByLeadHandler);

        // POST
        interactions.MapPost("", CreateInteractionHandler);                             // create interaction (summary/transcript optional)
        interactions.MapPost("/{id:guid}/attach-transcript", AttachTranscriptHandler); // attach transcript text

        // PATCH
        interactions.MapPatch("/{id:guid}", UpdateInteractionHandler);                  // update summary/sentiment/transcript

        // DELETE
        interactions.MapDelete("/{id:guid}", DeleteInteractionHandler);
    }

    // GET
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

    // POST
    private static IResult CreateInteractionHandler()
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult AttachTranscriptHandler(Guid id)
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    // PATCH
    private static IResult UpdateInteractionHandler(Guid id)
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    // DELETE
    private static IResult DeleteInteractionHandler(Guid id)
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }
}