
namespace Thrarin.Storage
{
    public abstract class EntityFrameworkSeed<T> where T : EntityFrameworkContext
    {
        protected T EntityFrameworkContext => this.entityFrameworkContext; 
        private readonly T entityFrameworkContext;
        protected EntityFrameworkSeed(T entityFrameworkContext)
        {
            this.entityFrameworkContext = entityFrameworkContext;
        }

        protected abstract void SeedForDevelopment();
        protected abstract void SeedForStaging();
        protected abstract void SeedForProduction();

        public void Seed(string environment = "")
        {
            switch (environment.ToLower())
            {
                case "production": this.SeedForProduction(); break;
                case "staging": this.SeedForStaging(); break;
                default: this.SeedForDevelopment(); break;
            }
            this.entityFrameworkContext.SaveChanges();
        }
    }
}
