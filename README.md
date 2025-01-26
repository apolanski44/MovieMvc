# **Movie Review Application Documentation**

## **Overview**

This application allows users to browse, review, and rate movies. Users can create an account, log in, and engage with the platform by adding movies to their favorites, leaving reviews, and exploring top-rated movies. Administrators have the ability to manage users and movies.

---

## **Key Features**

### **For Users:**

- **Browse Movies**: View a comprehensive list of movies along with their details such as title, genre, release year, description, and average rating.
- **Rate Movies**: Submit ratings for movies on a scale of 1 to 5.
- **Write Reviews**: Share your opinions about movies.
- **Favorites**: Add movies to your personal favorites list for easy access.
- **Notifications**: Receive notifications about newly added movies.

### **For Administrators:**

- **Manage Movies**: Add, edit, or delete movies in the database.
- **Manage Users**: View, edit, or delete user accounts.
- **Top-Rated Movies**: View a list of the top 10 highest-rated movies.

---

## **Database Structure**

### **Movies**

- **Description**: Stores details about movies.
- **Columns**:
  - `Id` (PK, int) – Unique identifier for the movie.
  - `Title` (string) – Title of the movie.
  - `Genre` (string) – Genre of the movie.
  - `Description` (string) – Brief description of the movie.
  - `ReleaseYear` (int) – Release year of the movie.
  - `ImagePath` (string) – Path to the movie poster.
  - `AverageRating` (float) – Calculated average rating of the movie.
- **Relationships**:
  - One-to-many relationship with **Ratings** and **Comments**.

---

### **Ratings**

- **Description**: Stores user ratings for movies.
- **Columns**:
  - `Id` (PK, int) – Unique identifier for the rating.
  - `UserId` (FK, string) – References the user who gave the rating.
  - `MovieId` (FK, int) – References the movie being rated.
  - `Score` (int) – Rating score (1 to 5).
- **Relationships**:
  - Many-to-one relationship with **Movies**.
  - Many-to-one relationship with **Users**.

---

### **Comments**

- **Description**: Stores user reviews for movies.
- **Columns**:
  - `Id` (PK, int) – Unique identifier for the comment.
  - `UserId` (FK, string) – References the user who wrote the comment.
  - `MovieId` (FK, int) – References the movie being reviewed.
  - `Content` (string) – Text content of the review.
  - `CreatedAt` (datetime) – Timestamp of when the comment was created.
- **Relationships**:
  - Many-to-one relationship with **Movies**.
  - Many-to-one relationship with **Users**.

---

### **Favorites**

- **Description**: Tracks movies added to users' favorites.
- **Columns**:
  - `Id` (PK, int) – Unique identifier for the favorite record.
  - `UserId` (FK, string) – References the user.
  - `MovieId` (FK, int) – References the movie.

---

### **Notifications**

- **Description**: Stores notifications for users.
- **Columns**:
  - `Id` (PK, int) – Unique identifier for the notification.
  - `UserId` (FK, string) – References the user.
  - `Message` (string) – Notification message.
  - `CreatedAt` (datetime) – Timestamp of when the notification was created.
- **Relationships**:
  - Many-to-one relationship with **Users**.

---

### **Users**

- **Description**: Managed by ASP.NET Identity for authentication and authorization.
- **Columns**:
  - `Id` (PK, string) – Unique identifier for the user.
  - `UserName` (string) – Username of the user.
  - `Email` (string) – Email address of the user.
  - **Other ASP.NET Identity-related columns.**

---

## **Stored Procedures, Functions, and Triggers**

### **Stored Procedure: GetMoviesByGenre**

- **Description:** Retrieves movies by their genre.
- **Definition**:
  ```sql
  CREATE PROCEDURE GetMoviesByGenre
      @Genre NVARCHAR(MAX)
  AS
  BEGIN
      IF @Genre = 'All'
      BEGIN
          SELECT 
              Movies.Id,
              Movies.Title,
              Movies.Genre,
              Movies.Description,
              Movies.ReleaseYear,
              Movies.ImagePath,
              ISNULL(AVG(CAST(Ratings.Score AS FLOAT)), 0) AS AverageRating
          FROM Movies
          LEFT JOIN Ratings ON Movies.Id = Ratings.MovieId
          GROUP BY Movies.Id, Movies.Title, Movies.Genre, Movies.Description, Movies.ReleaseYear, Movies.ImagePath
      END
      ELSE
      BEGIN
          SELECT 
              Movies.Id,
              Movies.Title,
              Movies.Genre,
              Movies.Description,
              Movies.ReleaseYear,
              Movies.ImagePath,
              ISNULL(AVG(CAST(Ratings.Score AS FLOAT)), 0) AS AverageRating
          FROM Movies
          LEFT JOIN Ratings ON Movies.Id = Ratings.MovieId
          WHERE Movies.Genre = @Genre
          GROUP BY Movies.Id, Movies.Title, Movies.Genre, Movies.Description, Movies.ReleaseYear, Movies.ImagePath
      END
  END
  ```

---

### **Function: GetTopRatedMovies**

- **Description:** Returns the top 10 highest-rated movies, ordered by average rating and title.
- **Definition**:
  ```sql
  ALTER FUNCTION [dbo].[GetTopRatedMovies]()
  RETURNS TABLE
  AS
  RETURN
  (
      SELECT TOP 10 
          m.Id,
          m.Title,
          m.Genre,
          m.Description,
          m.ReleaseYear,
          m.ImagePath,
          CAST(ISNULL(AVG(CAST(r.Score AS FLOAT)), 0) AS FLOAT) AS AverageRating
      FROM Movies m
      LEFT JOIN Ratings r ON m.Id = r.MovieId
      GROUP BY 
          m.Id, 
          m.Title, 
          m.Genre, 
          m.Description, 
          m.ReleaseYear, 
          m.ImagePath
      ORDER BY AverageRating DESC, Title ASC
  );
  ```

---

### **Trigger: trg_AddMovieNotification**

- **Description:** Automatically generates notifications for all users when a new movie is added to the Movies table.
- **Definition**:
  ```sql
  CREATE TRIGGER trg_AddMovieNotification
  ON Movies
  AFTER INSERT
  AS
  BEGIN
      SET NOCOUNT ON;

      INSERT INTO Notifications (UserId, Message, CreatedAt)
      SELECT 
          u.Id,
          CONCAT('A new movie "', i.Title, '" has been added!'),
          GETDATE()
      FROM 
          AspNetUsers u
      CROSS JOIN 
          inserted i;
  END;
  
## **Controllers Overview**

### **AccountController**
- Manages user authentication and account-related functionality.
- Key Actions:
  - Login
  - Registration
  - Logout

### **AdminMoviesController**
- Allows administrators to manage movies.
- Key Actions:
  - Add a new movie
  - Edit movie details
  - Delete movies

### **AdminUsersController**
- Handles user management for administrators.
- Key Actions:
  - View user list
  - Edit user roles
  - Delete users

### **FavoritesController**
- Manages user favorite movies.
- Key Actions:
  - Add a movie to favorites
  - View favorite movies
  - Remove a movie from favorites

### **HomeController**
- Manages general pages for the application.
- Key Actions:
  - Display the home page
  - Show privacy policy

### **MovieController**
- Handles movie-related functionality.
- Key Actions:
  - Display the list of movies
  - Filter movies by genre
  - Rate movies

### **NotificationsController**
- Handles user notifications.
- Key Actions:
  - Display notifications
  - Mark notifications as read (if implemented)

### **ReviewsController**
- Manages movie reviews.
- Key Actions:
  - Add a review
  - View reviews for a movie
  - Delete reviews (if user is the owner)

---


## **Technologies Used**

- **Frontend**:
  - HTML5, CSS3, Bootstrap: For designing responsive and user-friendly interfaces.
- **Backend**:
  - ASP.NET Core MVC: For building the application logic and handling HTTP requests.
- **Database**:
  - Microsoft SQL Server: For storing movies, ratings, reviews, and user information.
  - Entity Framework Core: For object-relational mapping (ORM).
- **Authentication & Authorization**:
  - ASP.NET Identity: For user management and role-based access.
- **Additional Tools**:
  - Visual Studio: For development and debugging.
  - Git: For version control.

---

## **Security Features**

1. **Authentication**:
   - ASP.NET Identity is used to manage user authentication.
   - Passwords are hashed and securely stored.
   - Login sessions are managed using cookies with secure and HTTP-only flags.

2. **Authorization**:
   - Role-based access control (RBAC) ensures different privileges for administrators and regular users.
   - Secure areas (e.g., admin panels) are accessible only to users with the correct roles.

3. **Data Validation**:
   - All user inputs are validated on both client and server sides to prevent SQL injection and XSS attacks.

4. **Session Management**:
   - Session timeouts are implemented to automatically log out inactive users.
   - Sensitive data, like user preferences (e.g., selected genre), is stored in sessions to enhance user experience.

