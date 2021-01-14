using System;
using System.Threading.Tasks;
using AutoMapper;

namespace Bfm.Diet.Core.Mapper
{
    public static class DietMapper
    {
        private const string InvalidOperationMessage =
            "Mapper not initialized.";

        private const string AlreadyInitialized =
            "Mapper already initialized.";

        private static IConfigurationProvider _configuration;
        private static IMapper _instance;

        private static IConfigurationProvider Configuration
        {
            get => _configuration ?? throw new InvalidOperationException(InvalidOperationMessage);
            set => _configuration =
                _configuration == null ? value : throw new InvalidOperationException(AlreadyInitialized);
        }

        public static IMapper Mapper
        {
            get => _instance ?? throw new InvalidOperationException(InvalidOperationMessage);
            private set => _instance = value;
        }

        public static void Initialize(Action<IMapperConfigurationExpression> config)
        {
            Initialize(new MapperConfiguration(config));
        }

        public static void Initialize(MapperConfiguration config)
        {
            Configuration = config;
            Mapper = Configuration.CreateMapper();
        }

        public static void AssertConfigurationIsValid()
        {
            Configuration.AssertConfigurationIsValid();
        }

        public static Task<TResult> MapAsync<TSource, TResult>(this IMapper mapper, Task<TSource> task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));

            var tcs = new TaskCompletionSource<TResult>();

            task
                .ContinueWith(t => tcs.TrySetCanceled(), TaskContinuationOptions.OnlyOnCanceled);

            task
                .ContinueWith
                (
                    t => { tcs.TrySetResult(mapper.Map<TSource, TResult>(t.Result)); },
                    TaskContinuationOptions.OnlyOnRanToCompletion
                );

            task
                .ContinueWith
                (
                    t => tcs.TrySetException(t.Exception),
                    TaskContinuationOptions.OnlyOnFaulted
                );

            return tcs.Task;
        }
    }
}