using MicroservicePrecos;
using MicroservicePrecos.Infra.Contexto;

namespace MicroserviceVendas.Infra
{
    public static class GeradorDeServicos
    {
        public static ServiceProvider ServiceProvider;

        public static PrecosContext CarregarContexto()
        {
            return ServiceProvider.GetService<PrecosContext>();
        }
    }
}