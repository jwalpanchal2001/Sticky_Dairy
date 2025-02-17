using Microsoft.AspNetCore.Authentication.Cookies;
using Sticky_Dairy.Application.Services;
using Microsoft.EntityFrameworkCore;
using Sticky_Dairy.Infrastructure.Data;
using Sticky_Dairy.Application.Interfaces;
using Sticky_Dairy.Infrastructure.Repositories;
using Sticky_Dairy.Application.Interface;
using StickyDiary.Application.Interfaces;
using StickyDiary.Infrastructure.Repositories;
using Sticky_Dairy.Infrastructure.Repository.Notes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Sticky_Dairy_dbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IStickyNoteRepository, StickyNoteRepository>();
builder.Services.AddScoped<IAttachmentRepository, AttachmentRepository>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<IReminderRepository, ReminderRepository>();



builder.Services.AddScoped<IStickyNoteService, StickyNoteService>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<IUserService, UserService>();



builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);



// Add cookie authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login"; // Redirect to login page if unauthorized
        options.LogoutPath = "/User/Logout";
        options.AccessDeniedPath = "/User/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Session timeout
        options.SlidingExpiration = true; // Reset expiration time if active
    });
builder.Services.AddAuthorization(); // 🔹 This is required!
builder.Services.AddSession();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession(); // 🔹 Add session middleware

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}");

app.Run();
