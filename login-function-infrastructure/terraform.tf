provider "aws" {
  region = var.region
}

terraform {
  backend "s3" {
    bucket = local.s3_infrastructure-bucket_name
    key    = "authorization/login/function/state/login-function.tfstate"
    region = var.region
  }
}

variable "environment" {
  description = "Environment name - dev, sbx, prd"
}

variable "region" {
  description = "AWS Region"
}

locals {
  s3_infrastructure-bucket_name = "aspharax-infrastructure"
  secret-name                   = "MatchUpSiteUserPoolCredentials"
}
