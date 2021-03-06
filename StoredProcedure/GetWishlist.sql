USE [BookStore]
GO
/****** Object:  StoredProcedure [dbo].[RemoveFromWishList]    Script Date: 11/20/2021 9:02:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[RemoveFromWishList]
@WishListId INT ,
@result int output
AS
BEGIN
BEGIN TRY
Delete FROM WishList Where WishListId =@WishListId;
set @result=1;
END TRY
BEGIN CATCH
set @result=0;
END CATCH
END


Create PROC GetWishList
(@userId INT)
AS
BEGIN
select 
Books.Id,BookName,AuthorName,Price,OriginalPrice,Image,WishListId
FROM Books
inner join WishList
on WishList.BookId=Books.Id where WishList.UserId=@userId
END