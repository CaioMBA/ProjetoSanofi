import 'package:sanofi_app/Domain/DefaultApiResponseModel.dart';
import './UserModels.dart';

class AuthTokenResponse extends DefaultApiResponseModel{
  final String? token;
  final DateTime? creation;
  final DateTime? expiration;
  final UserInfo? userInfo;

  AuthTokenResponse({this.token, this.creation, this.expiration, this.userInfo, required super.success, required super.message});
}