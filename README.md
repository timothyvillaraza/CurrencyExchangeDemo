
# Currency Exchange Web Application

This is a simple web application built using **ASP.NET Core MVC** and targeting **.NET 8.0**. The application provides an interface for accessing and interacting with real-time currency exchange rates.

## Live Application Demo

You can access the deployed application at the following URL:  
[Currency Exchange Demo](https://currencyexchangedemo.azurewebsites.net/)

---

## External API Utilization

This application utilizes the following third-party API for fetching exchange rate data:  
[Currency Exchange API](https://github.com/fawazahmed0/exchange-api)

### **Important Note**
The **Currency Exchange API** may throttle the frequency of currency fetches. If you encounter issues with fetching data, this could be due to rate limiting by the API provider.

---

## Technologies Used

- **ASP.NET Core MVC**: Backend framework.
- **.NET 8.0**: Target framework.
- **Azure**: Hosting platform for the deployed application.

---

## How to Run Locally

1. **Clone the Repository**:  
   Clone the repository to your local machine.
   ```bash
   git clone <repository-url>
   cd <repository-folder>
2. **Install .NET SDK**:  
   Make sure you have the .NET 8 SDK installed. You can download it from [Microsoft's .NET website](https://dotnet.microsoft.com/).

3. **Run the Application**:  
   Use the following command to run the application:
   ```bash
   dotnet run
4. **Access Locally**:  
   The console should generae a link to the localhost and port.
