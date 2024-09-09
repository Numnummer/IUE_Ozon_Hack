namespace ApiResponse.Infrastructure
{
    public static class DbExtension
    {
        public static void AddCassandraContext(this WebApplicationBuilder builder, string tableName)
        {
            builder.Services.AddSingleton(
                new CassandraDbContext(builder.Configuration));
            builder.Services.AddSingleton(
                provider => provider.GetService<CassandraDbContext>().GetTableSession(tableName));
        }
    }
}
