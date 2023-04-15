using ServiceMergeFile.Core;

namespace ServiceMergeFile
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Регистрация сервисов проекта
        /// </summary>
        /// <param name="services"></param>
        public static void AddService(this IServiceCollection services)
        {
            services.AddTransient<IMergeFile, MergeFile>();
            services.AddTransient<ICheckedRowText, CheckedRowText>();
            services.AddTransient<IResultCheckedText, ResultCheckedTextMessage>();
            services.AddTransient<IFileService, FileService>();

        }
    }
}
