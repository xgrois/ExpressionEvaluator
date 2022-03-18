# ExpressionEvaluator

A math web API to perform some calculations.

Example:

```bash
curl -X 'POST' \
  'https://localhost:7283/evaluate' \
  -H 'accept: application/json' \
  -H 'Content-Type: application/json' \
  -d '{
  "expression": "25*(3-2)^2"
}'
```

You can pass your math expression in the body

```json
{
  "expression": "25*(3-2)^2"
}
```

and you will get the response

```json
{
  "expression": "25*(3-2)^2",
  "result": 25
}
```
