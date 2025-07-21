namespace FoodFlow.Mapping
{
    public class MappingConfigurations : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RegisterRequest, ApplicationUser>()
                 .Map(dest => dest.UserName, src => src.Email);


        }
    }
}
