--Create a stored proc pr_GetOrderSummary

CREATE PROCEDURE pr_GetOrderSummary
    @StartDate DATE,
    @EndDate DATE,
    @EmployeeID INT = NULL,
    @CustomerID VARCHAR(30) = NULL
AS
BEGIN
    SELECT
        CONCAT(E.TitleOfCourtesy, ' ', E.FirstName, ' ', E.LastName) AS EmployeeFullName,
        S.CompanyName AS ShipperCompanyName,
        C.CompanyName AS CustomerCompanyName,
        COUNT(O.OrderID) AS NumberOfOrders,
        MIN(O.OrderDate) AS [Date],
        SUM(O.Freight) AS TotalFreightCost,
        COUNT(DISTINCT OD.ProductID) AS NumberOfDifferentProducts,
        SUM(OD.UnitPrice * OD.Quantity) AS TotalOrderValue
    FROM
        Orders O
        JOIN Employees E ON O.EmployeeID = E.EmployeeID
        JOIN Shippers S ON O.ShipVia = S.ShipperID
        JOIN Customers C ON O.CustomerID = C.CustomerID
        JOIN [Order Details] OD ON O.OrderID = OD.OrderID
    WHERE
        (O.OrderDate BETWEEN @StartDate AND @EndDate)
        AND (@EmployeeID IS NULL OR O.EmployeeID = @EmployeeID)
        AND (@CustomerID IS NULL OR O.CustomerID = @CustomerID)
    GROUP BY
        CONCAT(E.TitleOfCourtesy, ' ', E.FirstName, ' ', E.LastName),
        S.CompanyName,
        C.CompanyName
    ORDER BY
        MIN(O.OrderDate),
        CONCAT(E.TitleOfCourtesy, ' ', E.FirstName, ' ', E.LastName),
        S.CompanyName,
        C.CompanyName;
END;


--Testing scripts

--exec pr_GetOrderSummary @StartDate='1 Jan 1996', @EndDate='31 Aug 1996', @EmployeeID=NULL , @CustomerID=NULL

--exec pr_GetOrderSummary @StartDate='1 Jan 1996', @EndDate='31 Aug 1996', @EmployeeID=5 , @CustomerID=NULL

--exec pr_GetOrderSummary @StartDate='1 Jan 1996', @EndDate='31 Aug 1996', @EmployeeID=NULL , @CustomerID='VINET'

--exec pr_GetOrderSummary @StartDate='1 Jan 1996', @EndDate='31 Aug 1996', @EmployeeID=5 , @CustomerID='VINET'