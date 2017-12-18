namespace csharp userthrift.itf

struct User
{
	1: i32 userId,
    2: string userName,
    3: string trueName
}

struct address
{
	1: string Line1,
	2: string Line2
}

struct Person
{
	1: i32 Id,
	2: string Name,
	3: address Address
}

service UserService{

    string GetUserName(1:i32 userId)

    bool Login(1:string username, 2:string psd)

	i32 Add(1:i32 userId, 2:string userName)

	User GetUserInfo(1:i32 userId)
}
