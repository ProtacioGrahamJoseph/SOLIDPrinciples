using System;

namespace SOLIDPrinciples_
{
	// ISP - Interface Segregation Principle: Separate interfaces for Authentication and User Notification
	public interface IAuthentication
	{
		bool Authenticate(string username, string password);
	}

	public interface IUserNotification
	{
		void NotifyUser(string message);
	}

	// SRP - Single Responsibility Principle: Handles only user authentication
	public class SimpleAuthentication : IAuthentication
	{
		public bool Authenticate(string username, string password)
		{
			return username == "admin" && password == "password123";
		}
	}

	// OCP - Open/Closed Principle: Can add new authentication mechanisms without modifying existing ones
	public class OAuthAuthentication : IAuthentication
	{
		public bool Authenticate(string username, string password)
		{
			return username == "oauthUser" && password == "oauthPassword";
		}
	}

	// SRP - Single Responsibility Principle: Handles only user notifications
	public class EmailNotification : IUserNotification
	{
		public void NotifyUser(string message)
		{
			Console.WriteLine($"Email Notification: {message}");
		}
	}

	// OCP - Adding a new notification method without altering existing ones
	public class SMSNotification : IUserNotification
	{
		public void NotifyUser(string message)
		{
			Console.WriteLine($"SMS Notification: {message}");
		}
	}

	// LSP - Works with any IAuthentication and IUserNotification implementations
	public class AuthenticationService
	{
		private readonly IAuthentication _authentication;
		private readonly IUserNotification _notification;

		// DIP - Dependencies are passed via abstraction (IAuthentication, IUserNotification)
		public AuthenticationService(IAuthentication authentication, IUserNotification notification)
		{
			_authentication = authentication;
			_notification = notification;
		}

		public void Login(string username, string password)
		{
			if (_authentication.Authenticate(username, password))
			{
				_notification.NotifyUser("Login Successful!");
			}
			else
			{
				_notification.NotifyUser("Login Failed.");
			}
		}
	}

	// Main method as a separate static method
	internal static class Program
	{
		static void Main(string[] args)
		{
			IAuthentication simpleAuth = new SimpleAuthentication();
			IAuthentication oauthAuth = new OAuthAuthentication();

			IUserNotification emailNotifier = new EmailNotification();
			IUserNotification smsNotifier = new SMSNotification();

			var simpleAuthService = new AuthenticationService(simpleAuth, emailNotifier);
			var oauthAuthService = new AuthenticationService(oauthAuth, smsNotifier);

			simpleAuthService.Login("admin", "password123");
			oauthAuthService.Login("oauthUser", "oauthPassword");

			Console.ReadLine();
		}
	}
}
