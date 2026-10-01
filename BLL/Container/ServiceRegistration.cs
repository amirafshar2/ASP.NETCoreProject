using BLL.Abstract;
using BLL.Concrete;
using DAL.Abstract;
using DAL.EntityFramework;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Container
{
    /// <summary>Registriert alle Repositories und Manager im DI-Container (Scoped = ein Context pro Request).</summary>
    public static class ServiceRegistration
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IBlogDAL, EfBlogRepository>();
            services.AddScoped<IBlogRatingDAL, EfBlogRatingRepository>();
            services.AddScoped<ICategoryDAL, EfCategoryRepository>();
            services.AddScoped<ICommentDAL, EfCommentRepository>();
            services.AddScoped<IWriterDAL, EfWriterRepository>();
            services.AddScoped<IMessageDAL, EfMessageRepository>();
            services.AddScoped<IContactDAL, EfContactRepository>();
            services.AddScoped<INewsLetterDAL, EfNewsLetterRepository>();
            services.AddScoped<INotificationDAL, EfNotificationRepository>();
            services.AddScoped<IAboutDAL, EfAboutRepository>();

            services.AddScoped<IBlogService, BlogManager>();
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<ICommentService, CommentManager>();
            services.AddScoped<IWriterService, WriterManager>();
            services.AddScoped<IMessageService, MessageManager>();
            services.AddScoped<IContactService, ContactManager>();
            services.AddScoped<INewsLetterService, NewsLetterManager>();
            services.AddScoped<INotificationService, NotificationManager>();
            services.AddScoped<IAboutService, AboutManager>();
            services.AddScoped<IDashboardService, DashboardManager>();
            return services;
        }
    }
}
