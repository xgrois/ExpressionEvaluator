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
## Using variables

### You can set variables...

Example:

```bash
curl -X 'POST' \
  'https://localhost:7283/variables' \
  -H 'accept: application/json' \
  -H 'Content-Type: application/json' \
  -d '{
  "name": "x",
  "value": 5
}'
```

You can pass your math expression in the body

```json
{
  "name": "x",
  "value": 5
}
```

### and then use for evaluations, 

```bash
curl -X 'POST' \
  'https://localhost:7283/evaluate' \
  -H 'accept: application/json' \
  -H 'Content-Type: application/json' \
  -d '{
  "expression": "x*2+1"
}'
```

You can pass your math expression in the body

```json
{
  "expression": "x*2+1"
}
```

and you will get the response

```json
{
  "expression": "x*2+1",
  "result": 11
}
```
