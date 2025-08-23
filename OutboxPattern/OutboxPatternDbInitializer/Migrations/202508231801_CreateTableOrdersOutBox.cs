using FluentMigrator;

namespace OutboxPatternDbInitializer.Migrations
{

    [Migration(202508231801)]
    public class _202508231801_CreateTableOrdersOutBox : Migration
    {
        public override void Up()
        {
            Create.Table("orders_outbox")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("order_id").AsGuid().NotNullable()
                .WithColumn("type").AsString(100).NotNullable()
                .WithColumn("content").AsString().NotNullable()
           .WithColumn("created_at").AsDateTime().NotNullable()
            .WithColumn("processed_on_utc").AsDateTime().Nullable()
             .WithColumn("updated_at").AsDateTime().Nullable()
            .WithColumn("error").AsString().Nullable();
        }

        public override void Down()
        {
            Delete.Table("orders_outbox");
        }
    }
}