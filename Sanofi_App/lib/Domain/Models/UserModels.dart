class UserInfo{
    final double userID;
    final String name;
    final String login;
    final DateTime dateCreation;
    final bool active;

    UserInfo({required this.userID, required this.name, required this.login, required this.dateCreation, required this.active});

    factory UserInfo.fromJson(Map<String, dynamic> json){
        return UserInfo(
            userID: json['userID'],
            name: json['name'],
            login: json['login'],
            dateCreation: DateTime.parse(json['dateCreation']),
            active: json['active']
        );
    }
}