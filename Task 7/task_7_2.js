function mathCalculator(str, fixed = 2) {
  if (str.constructor.name !== "String")
    throw Error("First arg must be a string!");

  const splitted = str.split(""),
    opers = "+*-/=";

  let right = "",
    left = undefined,
    oper = "";

  splitted.forEach((char) => {
    if (opers.includes(char)) {
      if (left && right) {
        left = doAction(left, right, oper);
      } else left = +right;

      oper = char;
      right = "";
    } else {
      right += char;
    }
  });

  function doAction(oper1, oper2, action) {
    let n1 = +oper1,
      n2 = +oper2;

    switch (action) {
      case "+":
        return n1 + n2;
      case "-":
        return n1 - n2;
      case "/":
        return n1 / n2;
      case "*":
        return n1 * n2;
    }
  }

  return Number.isInteger(left) ? left : left.toFixed(fixed);
}

let str = "3.5 +4*10-5.3 /5 =   ";

let output = mathCalculator(str);

console.log(output);
