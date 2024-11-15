using MicroserviceVendas;
using MicroserviceVendas.Infra.Contexto;

namespace MicroserviceVendas.Infra
{
    public static class GeradorDeServicos
    {
        public static ServiceProvider ServiceProvider;

        public static VendasContext CarregarContexto()
        {
            return ServiceProvider.GetService<VendasContext>();
        }
    }
}