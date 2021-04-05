using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AdaptiveDialogBot.BotApp.IntegrationTests.EchoBotTests
{
    public class EchoBotShould : IClassFixture<BotApplicationFactory>, IDisposable
    {
        private bool _disposedValue;
        private readonly IServiceScope _scope;

        public EchoBotShould(BotApplicationFactory applicationFactory)
        {
            _scope = applicationFactory.CreateScope();
        }

        [Fact]
        public async Task Reply_back_whatever_it_receives()
        {
            // Arrange -----------------

            // Act ---------------------

            // Assert ------------------

        }

        private T GetService<T>() => _scope.ServiceProvider.GetRequiredService<T>();

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _scope.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
