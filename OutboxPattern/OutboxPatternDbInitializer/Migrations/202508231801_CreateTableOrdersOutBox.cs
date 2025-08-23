using FluentMigrator;

namespace OutboxPatternDbInitializer.Migrations
{

    [Migration(202508231801)]
    public class _202508231801_CreateTableOrdersOutBox : Migration
    {
        public override void Up()
        {
            Create.Table("orders_outbox")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("type").AsString(100).NotNullable()
                .WithColumn("content").AsString().NotNullable()
            .WithColumn("occurred_on_utc").AsDateTime().NotNullable()
            .WithColumn("processed_on_utc").AsDateTime()
            .WithColumn("error").AsString();
        }

        public override void Down()
        {
            Delete.Table("orders_outbox");
        }
    }
}