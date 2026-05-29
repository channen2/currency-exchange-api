# currency-exchange-api

One time setup
dotnet tool restore
dotnet husky install


## Configuration

1. Set up environment file

Create a `.env` file in the root of the project and change the SA_PASSWORD in the corresponding locations.

```bash
cp .env.example .env
```

2. Run the application

```bash
docker compose up --build
```

3. Swagger UI is available at `http://localhost:5000`

4. Stop the application

To remove all persisted data use `-v` flag

```bash
docker compose down
```
