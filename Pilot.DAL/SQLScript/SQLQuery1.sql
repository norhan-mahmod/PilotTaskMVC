-- Create Tables
create table IssueType(
	Id int primary key identity(1,1),
	Name nvarchar(20) not null
);

create table CustomerTickets(
	Id int primary key identity(1,1),
	FullName nvarchar(max) not null,
	MobileNumber nvarchar(20) not null,
	Email nvarchar(max) not null,
	IssueTypeId int references IssueType(Id),
	Description nvarchar(max) not null,
	Priority int not null,
	Status nvarchar(10) default 'Open',
	CreatedAt datetime default getdate()
);

-- Seeding Data in IssueType table
insert into IssueType 
values ('Technical' ),( 'Billing') ,( 'Complaint') ,( 'Other')

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

--Get Tickets with optional filter with IssueType , Priority
create procedure SP_GetTickets 
				@IssueTypeId int = null,
				@Priority int = null
with encryption as
begin
	if(@IssueTypeId is null and @Priority is null)
		select * from CustomerTickets
	else if(@IssueTypeId is not null)
		select * from CustomerTickets where IssueTypeId = @IssueTypeId
	else if(@Priority is not null)
		select *from CustomerTickets where Priority = @Priority
end

--Get Ticket By Id
create procedure SP_GetTicketById @Id int
with encryption as
begin
	select * from CustomerTickets where Id = @Id
end

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

-- Get Issue Types
create procedure SP_GetIssueTypes
with encryption as
begin
	select * from IssueType
end
