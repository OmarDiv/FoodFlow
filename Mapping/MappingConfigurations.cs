namespace FoodFlow.Mapping
{
    public class MappingConfigurations : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //config.NewConfig<Restaurant, RestaurantListResponse>()
            // .Map(des => des.IsOpen, src => src.IsActive);
        }
    }
}
