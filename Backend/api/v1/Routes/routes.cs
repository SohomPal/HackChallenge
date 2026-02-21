namespace api.v1.Routes;

public static class Routes
{
    public static void MapTodoRoutes(this IEndpointRouteBuilder app){
        RouteGroupBuilder todoRoutes = app.MapGroup("/api/v1/todos");

        //GET routes.
        // todoRoutes.MapGet("/get-todos", GetTodosHandler);
        // todoRoutes.MapGet("/get-completed-todos", GetCompletedTodosHandler);
        // todoRoutes.MapGet("/get-incompleted-todos", GetIncompletedTodosHandler);
        // todoRoutes.MapGet("/get-todo/{id}", GetTodoByIdHandler);

        // //POST route.
        // todoRoutes.MapPost("/add-todo", AddTodoHandler);

        // //PATCH routes
        // todoRoutes.MapPatch("/update-todo-complete-status/{id}", UpdateTodoByidHandler);
        // todoRoutes.MapPatch("/update-todoName/{id}", UpdateTodoByNameHandler);

        // //Delete Route
        // todoRoutes.MapDelete("/delete-todo/{id}", DeleteTodoByIdHandler);
    }   
}