-- Create Tables
create table IssueTypes(
	Id int primary key identity(1,1),
	Name nvarchar(20) not null
);

create table CustomerTickets(
	Id int primary key identity(1,1),
	FullName nvarchar(max) not null,
	MobileNumber nvarchar(20) not null,
	Email nvarchar(max) not null,
	IssueTypeId int references IssueTypes(Id),
	Description nvarchar(max) not null,
	Priority int not null,
	Status nvarchar(10) default 'Open',
	CreatedAt datetime default getdate()
);
GO

-- Seeding Data in IssueType table
insert into IssueTypes 
values ('Technical' ),( 'Billing') ,( 'Complaint') ,( 'Other')
GO

-- Stored Procedures
--ADD Ticket
create procedure SP_AddTicket 
				@FullName nvarchar(max),
				@MobileNumber nvarchar(20),
				@Email nvarchar(max),
				@IsuueTypeId int,
				@Description nvarchar(max),
				@Priority int
with encryption as
begin
	insert into CustomerTickets(FullName,MobileNumber,Email,IssueTypeId,Description,Priority)
	values (@FullName,@MobileNumber,@Email,@IsuueTypeId,@Description,@Priority)
end
GO

--Get Tickets with optional filter with IssueType , Priority
create procedure SP_GetTickets 
				@IssueTypeId int = null,
				@Priority int = null
with encryption as
begin
		select * from CustomerTickets 
		where ( @IssueTypeId is null or IssueTypeId = @IssueTypeId) 
		and ( @Priority is null or Priority = @Priority)
end
GO

--Get Ticket By Id
create procedure SP_GetTicketById @Id int
with encryption as
begin
	select * from CustomerTickets where Id = @Id
end
GO

--Update Ticket 
create procedure SP_UpdateTicket 
				@Id int,
				@FullName nvarchar(max),
				@MobileNumber nvarchar(20),
				@Email nvarchar(max),
				@IsuueTypeId int,
				@Description nvarchar(max),
				@Priority int
with encryption as
begin
	update CustomerTickets
	set FullName = @FullName,
		MobileNumber = @MobileNumber,
		Email = @Email,
		IssueTypeId = @IsuueTypeId,
		Description = @Description,
		Priority = @Priority
	where Id = @Id
end
GO

-- Get Issue Types
create procedure SP_GetIssueTypes
with encryption as
begin
	select * from IssueTypes
end
GO
