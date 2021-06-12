class Service {
  constructor(collection = undefined) {
    let entries = null;

    if (collection) {
      if (collection.constructor.name === "Array") {
        entries = collection.map((item, index) => [index + "", item]);
      } else if (typeof collection[Symbol.iterator] === "function") {
        entries = collection;
      } else if (typeof collection === "object") {
        entries = Object.entries(collection);
      }
    }

    this.data = new Map(entries);
  }

  get length() {
    return this.data.size;
  }

  add(item, id = undefined) {
    if (!item) return null;

    let newId;

    if (id?.constructor.name === "String") newId = id;
    else newId = this.data.size + "";

    this.data.set(newId, item);

    return id;
  }

  getById(id) {
    if (!this.#isRightId(id)) return null;

    return this.data.get(id);
  }

  getAll() {
    return [...this.data.values()];
  }

  deleteById(id) {
    if (!this.#isRightId(id)) return null;

    let item = this.data.get(id);

    this.data.delete(id);

    return item;
  }

  updateById(id, newItem) {
    if (!this.#isRightId(id) || !this.#isObject(newItem)) return null;

    let item = this.data.get(id);

    this.#replaceItem(id, { ...item, ...newItem });
  }

  replaceById(id, newItem) {
    if (!this.#isRightId(id) || !newItem) return null;

    this.#replaceItem(id, newItem);
  }

  #isRightId(id) {
    if (id?.constructor.name !== "String" || !this.data.has(id)) return false;

    return true;
  }

  #isObject(id) {
    if (typeof id === "object") return true;

    return false;
  }

  #replaceItem(id, newItem) {
    this.data.delete(id);

    this.data.set(id, newItem);
  }
}


