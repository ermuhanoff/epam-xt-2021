function charRemover(str) {
  if (str.constructor.name !== "String")
    throw Error("First arg must be a string!");

  const splitted = str.split(""),
    seps = "?!:;,.\t ";

  let word = "",
    repeats = "";

  splitted.forEach((char) => {
    if (seps.includes(char)) {
      word = "";
    } else {
      if (word.includes(char)) {
        repeats += char;
      }
      word += char;
    }
  }, "");

  return splitted.filter((char) => !repeats.includes(char)).join("");
}

let output = charRemover("У попа была\tсобака");

console.log(output);
