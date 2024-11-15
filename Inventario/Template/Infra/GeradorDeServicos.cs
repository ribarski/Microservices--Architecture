using MicroserviceInventario;
using MicroserviceInventario.Infra.Contexto;

namespace MicroserviceInventario.Infra
{
    public static class GeradorDeServicos
    {
        public static ServiceProvider ServiceProvider;

        public static InventarioContext CarregarContexto()
        {
            return ServiceProvider.GetService<InventarioContext>();
        }
    }
}