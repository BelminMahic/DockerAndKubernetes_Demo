using Npgsql;

namespace DockerAndKubernetes.WebApi.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            using (var scope = host.Services.CreateScope())
            {
                var servicesCollection = scope.ServiceProvider;
                var configuration = servicesCollection.GetRequiredService<IConfiguration>();
                var logger = servicesCollection.GetRequiredService<ILogger<TContext>>();

                try
                {
                    logger.LogInformation("Migrating postgresql database.");

                    using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();

                    using var command = new NpgsqlCommand { Connection = connection };

                    command.CommandText = "DROP TABLE IF EXISTS TodoItem";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE TodoItem(Id SERIAL PRIMARY KEY,
                                                                Name VARCHAR(100) NOT NULL,
                                                                IsComplete BOOLEAN)";
                    command.ExecuteNonQuery();
                    command.CommandText = @"INSERT INTO TodoItem(Name,IsComplete) VALUES('Odrzati prezentaciju na FITu',true);";
                    command.ExecuteNonQuery();

                    command.CommandText = @"INSERT INTO TodoItem(Name,IsComplete) VALUES('Uciniti Docker i K8S zanimljivim', true);";
                    command.ExecuteNonQuery();

                    logger.LogInformation("Database has been successfully migrated.");

                }
                catch (NpgsqlException ex)
                {
                    logger.LogError(ex, "An error occured while migrating the postgresql database.");

                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, retryForAvailability);
                    }
                }
            }

            return host;
        }
    }
}
