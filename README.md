# PROG7311_PART_2

# Agri-Energy Connect
# Agri-Energy Connect is an ASP.NET Core MVC web application developed for academic purposes.
# It is designed to manage agricultural product data, enabling two distinct user roles: Farmers and Employees. 
# The system demonstrates user authentication, role-based access control, and database interaction using Entity Framework Core.

# Features
User registration and login with role selection

Role-based access:

Farmers can manage their own products

Employees can manage all farmers and view all products

CRUD operations for farmers and products

Filtering options on the product list by farmer, product type, and production date

Identity integration with confirmation email (using a fake email sender for testing)

SQL Server database managed with Entity Framework Core migrations

# Technologies Used
ASP.NET Core MVC (.NET 8)

Entity Framework Core

ASP.NET Identity

Razor Pages

SQL Server (LocalDB)

Bootstrap (for styling)

# User Roles

# Farmer:

Registers with email, password, and selects "Farmer" as role

Can view a personalized dashboard

Can add, edit, and view their own products

# Employee:

Registers with email, password, and selects "Employee" as role

Can view all products and filter them

Can manage farmer profiles including edit and delete actions

# Project Structure

Controllers

FarmerController.cs

ProductController.cs

Models

ApplicationUser.cs

Farmer.cs

Product.cs

Views

Farmer

Index.cshtml

Edit.cshtml

Product

Index.cshtml

Edit.cshtml

Areas

Identity

Pages

Account

Register.cshtml

Login.cshtml