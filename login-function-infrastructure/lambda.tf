resource "aws_lambda_function" "register-function_lambda-function" {
  function_name = local.function_name
  role          = aws_iam_role.register-function_iam-role.arn
  handler       = "UserLoginFunction::UserLoginFunction.Function::FunctionHandler"
  runtime       = "dotnet6"
  s3_bucket     = local.s3_infrastructure-bucket_name
  s3_key        = local.s3_lambda-function_key
  timeout       = 10

  environment {
    variables = {
      UserPoolAppSecret = local.user_pool_credentials_secret_name
    }
  }

  depends_on = [
    aws_iam_role_policy_attachment.register-function_cloudwatch-logging_attachment,
    aws_cloudwatch_log_group.lambda_log_group
  ]
}
