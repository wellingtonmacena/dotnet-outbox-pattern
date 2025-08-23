using FluentMigrator;

namespace OutboxPatternDbInitializer.Migrations
{
    [Migration(202508231800)]
    public class _202508231800_CreateTableOrders : Migration
    {
        public override void Up()
        {
            Create.Table("orders")
                .WithColumn("id").AsGuid().PrimaryKey()
                 .WithColumn("product_name").AsString(100).NotNullable()
                .WithColumn("customer_name").AsString(100).NotNullable()
                .WithColumn("quantity").AsInt32().NotNullable()
            .WithColumn("total_price").AsDecimal().NotNullable()
             .WithColumn("updated_at").AsDateTime().Nullable()
             .WithColumn("created_at").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("orders");
        }
    }


}
