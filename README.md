# Aspnet-crud
This repository is a simple crud using as main object cats.

## Deploy
To deploy, you need to add the environment variables to the project (currently, in the files *.env).
You can simply run using docker:
```bash
docker run --name aspnet-db --env_file=./db.env postgres:latest
docker build -t aspnet-api .;
docker run --name aspnet-api -p 3000:80 --env_file=./api.env --env_file=./db.env aspnet-api
```

## Routes
All routes are:
* POST / - Body=CatRequest
* GET /{id}
* PATCH /{id} - Body=CatRequest
* DELETE /{id}

Definitions:
```js
// CatRequest
{
  string name;
  string color;
}
```
