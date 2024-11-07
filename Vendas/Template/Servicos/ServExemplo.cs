using Exemplo;
using Template.Infra;

namespace Exemplo
{
    public interface IServExemplo
    {
        ExemploDTO Exemplo(int id);
    }

    public class ServExemplo : IServExemplo
    {
        private DataContext _dataContext;

        public ServExemplo()
        {
            _dataContext = GeradorDeServicos.CarregarContexto();
        }

        public ExemploDTO Exemplo(int id)
        {
            var exemploDto = new ExemploDTO()
            {
                Texto = "Hello world!"
            };

            return exemploDto;
        }
    }
}
