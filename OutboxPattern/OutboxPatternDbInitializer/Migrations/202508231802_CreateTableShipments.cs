using FluentMigrator;

namespace OutboxPatternDbInitializer.Migrations
{
    [Migration(202508231802)]
    public class _202508231802_CreateTableShipments : Migration
    {
        public override void Up()
        {
            Create.Table("shipments")
                .WithColumn("id").AsGuid().PrimaryKey()
                 .WithColumn("orders_id").AsGuid().NotNullable()
                .WithColumn("status").AsString(100).NotNullable()
                .WithColumn("updated_at").AsDateTime().Nullable()
             .WithColumn("created_at").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("shipments");
        }
    }
}
