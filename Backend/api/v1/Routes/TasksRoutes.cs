
namespace api.v1.Routes;

public static class TasksRoutes
{

    // ----------------------------
    // TASKS (follow-ups, alerts, automation)
    // ----------------------------
    public static void MapTasksRoutes(this IEndpointRouteBuilder app)
    {
        var tasks = app.MapGroup("/api/v1/tasks");

        // GET
        tasks.MapGet("", GetTasksHandler);                              // list tasks (filter: leadId, dueBefore, completed)
        tasks.MapGet("/{id:guid}", GetTaskByIdHandler);
        tasks.MapGet("/by-lead/{leadId:guid}", GetTasksByLeadHandler);
        tasks.MapGet("/due-today", GetTasksDueTodayHandler);            // dashboard widget
        tasks.MapGet("/overdue", GetOverdueTasksHandler);

        // POST
        tasks.MapPost("", CreateTaskHandler);                           // create task
        tasks.MapPost("/auto/after-demo/{leadId:guid}", ScheduleAfterDemoFollowUpsHandler); // schedules 3 follow-ups

        // PATCH
        tasks.MapPatch("/{id:guid}", UpdateTaskHandler);                // update description/dueDate
        tasks.MapPatch("/{id:guid}/complete", MarkTaskCompleteHandler); // toggle complete
        tasks.MapPatch("/{id:guid}/reopen", ReopenTaskHandler);

        // DELETE
        tasks.MapDelete("/{id:guid}", DeleteTaskHandler);
    }

    // GET
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

    // POST
    private static IResult CreateTaskHandler()
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    private static IResult ScheduleAfterDemoFollowUpsHandler(Guid leadId)
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }

    // PATCH
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

    // DELETE
    private static IResult DeleteTaskHandler(Guid id)
    {
        return Results.StatusCode(StatusCodes.Status501NotImplemented);
    }
}