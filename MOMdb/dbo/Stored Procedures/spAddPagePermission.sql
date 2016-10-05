CREATE proc [dbo].[spAddPagePermission]
@UserID          INT,
@Pages As [dbo].[tblTypePagePermission] Readonly
as

delete from tblPagePermissions where [User]=@UserID

insert into tblPagePermissions
([User], Page, [View], Access, Edit, [Add], [Delete])
select @userid, pageid,[view], access, edit, [add], [delete] from @Pages
