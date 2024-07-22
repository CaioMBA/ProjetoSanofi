import 'package:sanofi_app/Domain/DefaultApiResponseModel.dart';
import './UserModels.dart';

class AuthTokenResponse extends DefaultApiResponseModel{
  final String? token;
  final DateTime? creation;
  final DateTime? expiration;
  final UserInfo? userInfo;

  AuthTokenResponse({this.token, this.creation, this.expiration, this.userInfo, required super.success, super.message});

  factory AuthTokenResponse.fromJson(Map<String, dynamic> json) {
    final UserInfo? userInfo;
    if (json['userInfo'] == null){
      userInfo = null;
    }
    else{
      userInfo = UserInfo.fromJson(json['userInfo']);
    }

    return AuthTokenResponse(
      token: json['token'].toString(),
      creation: DateTime.parse(json['creation']),
      expiration: DateTime.parse(json['expiration']),
      userInfo: userInfo,
      success: json['success'],
      message: json['message']?.toString() ?? '',
    );
  }
}