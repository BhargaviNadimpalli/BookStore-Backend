use [BookStore]

create table Orders
(
         OrderId int not null identity (1,1) primary key,
	     BookId int,
		 UserId int,
		 OrderDate varchar(20),
		 CartId int
);
select * from [Orders]

create procedure PlaceOrder
 @BookId int,
 @UserId int,
 @OrderDate varchar(20),
 @CartId int,
 @result int output

 as
 begin
  BEGIN TRY
  BEGIN TRAN
	     begin
		   insert into Orders (BookId,UserId,OrderDate,CartId)
		                    
		                  values (@BookId,@UserId,@OrderDate,@CartId);
		   set @result=1
		   end
	    if(@result=1)
	      begin
	       delete from CartList where CartId=@CartId
		  Commit Tran
		end
  END TRY
  begin catch
      set @result=0;
	  Rollback Tran
  end catch
 end

 Create PROC GetOrder
(@userId INT)
AS
BEGIN
select 
Books.Id,BookName,AuthorName,Price,OriginalPrice,Image,OrderId
FROM Books
inner join Orders
on Orders.BookId=Books.Id where Orders.UserId=@userId
END