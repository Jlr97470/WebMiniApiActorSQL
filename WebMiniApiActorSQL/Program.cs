using Microsoft.Data;
using Microsoft.Data.SqlClient;
using WebMiniApiActorSQL;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddCors(opt => opt.AddDefaultPolicy(policy => policy.WithOrigins("")));
builder.Services.AddControllers().AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null);

var app = builder.Build();

app.MapPost("api/actor", (WebMiniApiActorSQL.Action actions) => {

    String connStr = "Data Source=localhost;Initial Catalog=SAKILA;Integrated Security=SSPI;TrustServerCertificate=true;";

    String strSQL;

    SqlCommand command;

    SqlDataReader reader;

    var Actors = new List<Actor>();

    if (actions.action != null)
    {

        using SqlConnection connection = new(connStr);

        try
        {
            connection.Open();

            switch (actions.action)
            {
                case "LIST":

                    try
                    {
                        command = new(@"SELECT * FROM Actor ORDER BY actor_id", connection);

                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Actors.Add(
                                new Actor()
                                {
                                    actor_id = reader.GetInt16(0),
                                    first_name = reader.GetString(1),
                                    last_name = reader.GetString(2),
                                    last_update = reader.GetDateTime(3).ToString("dd/MM/yyyy HH:mm:ss"),
                                    status = "ok",
                                    status_description = ""
                                }
                            );
                        }

                        if (Actors.Count == 0)
                        {
                            Actors.Add(
                            new Actor()
                            {
                                status = "ok",
                                status_description = ""
                            }
                            );

                            return Actors;
                        }

                        return Actors;
                    }
                    catch (Exception ex)
                    {

                        Actors.Add(
                        new Actor()
                        {
                            status = "ko",
                            status_description = ex.Message
                        }
                        );

                        return Actors;
                    }

                case "UPDATE":

                    try
                    {
                        strSQL = "UPDATE Actor SET ";

                        strSQL = strSQL + "first_name='" + (actions.first_name ?? "").Replace("'", "''") + "',";

                        strSQL = strSQL + "last_name='" + (actions.last_name ?? "").Replace("'", "''") + "',";

                        strSQL = strSQL + "last_update=CONVERT(DATETIME,'" + actions.last_update + "',103)";

                        strSQL = strSQL + " WHERE actor_id=" + actions.actor_id;

                        command = new(strSQL, connection);

                        command.ExecuteNonQuery();

                        Actors.Add(
                        new Actor()
                        {
                            status = "ok",
                            status_description = ""
                        }
                        );

                        return Actors;

                    }
                    catch (Exception ex)
                    {

                        Actors.Add(
                        new Actor()
                        {
                            status = "ko",
                            status_description = ex.Message
                        }
                        );

                        return Actors;
                    }
                case "ADD":

                    try
                    {
                        strSQL = "INSERT INTO Actor(";

                        strSQL = strSQL + "first_name,";

                        strSQL = strSQL + "last_name,";

                        strSQL = strSQL + "last_update)";

                        strSQL = strSQL + " VALUES('" + (actions.first_name ?? "").Replace("'", "''") + "',";

                        strSQL = strSQL + "'" + (actions.last_name ?? "").Replace("'", "''") + "',";

                        strSQL = strSQL + "CONVERT(DATETIME,'" + actions.last_update + "',103))";

                        command = new(strSQL, connection);

                        command.ExecuteNonQuery();

                        Actors.Add(
                        new Actor()
                        {
                            status = "ok",
                            status_description = ""
                        }
                        );

                        return Actors;

                    }
                    catch (Exception ex)
                    {

                        Actors.Add(
                        new Actor()
                        {
                            status = "ko",
                            status_description = ex.Message
                        }
                        );

                        return Actors;
                    }

                case "DELETE":

                    try
                    {
                        strSQL = "DELETE FROM Actor";

                        strSQL = strSQL + " WHERE actor_id=" + actions.actor_id;

                        command = new(strSQL, connection);

                        command.ExecuteNonQuery();

                        Actors.Add(
                        new Actor()
                        {
                            status = "ok",
                            status_description = ""
                        }
                        );

                        return Actors;

                    }
                    catch (Exception ex)
                    {

                        Actors.Add(
                        new Actor()
                        {
                            status = "ko",
                            status_description = ex.Message
                        }
                        );

                        return Actors;
                    }

                default:

                    Actors.Add(
                    new Actor()
                    {
                        status = "ko",
                        status_description = "ACTION NON GERER"
                    }
                    );

                    return Actors;

            }
        }
        catch (Exception ex)
        {

            Actors.Add(
            new Actor()
            {
                status = "ko",
                status_description = ex.Message
            }
            );

            return Actors;
        }
    }
    else
    {

        Actors.Add(
        new Actor()
        {
            status = "ko",
            status_description = "AUCUNE ACTION"
        }
        );

        return Actors;
    }
}
);

app.UseCors();

app.Run();
