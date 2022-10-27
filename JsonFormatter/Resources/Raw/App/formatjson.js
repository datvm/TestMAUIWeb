customElements.define("main-app", class extends HTMLElement {

    #txtMinified = this.querySelector(".txt-minified");
    #txtBeautified = this.querySelector(".txt-beautified");

    #btnBeautify = this.querySelector(".btn-beautify");
    #btnMinify = this.querySelector(".btn-minify");
    #btnDl = this.querySelector(".btn-download");

    constructor() {
        super();

        this.#btnBeautify.addEventListener("click", () => void this.#beautify());
        this.#btnMinify.addEventListener("click", () => void this.#minify());
        this.#btnDl.addEventListener("click", () => void this.#download());

        this.#testLoadJson();
    }

    async #testLoadJson() {
        const json = await fetch("./test.json").then(r => r.json());
        this.querySelector("#lbl-test").textContent = json.Text;
    }

    #download() {
        const file = new Blob([this.#txtBeautified.value], {
            type: "application/json",
        });
        const url = URL.createObjectURL(file);

        const a = document.createElement("a");
        a.href = url;
        a.download = "beautified.json";
        a.click();

        URL.revokeObjectURL(url);
    }

    #minify() {
        this.#format(this.#txtBeautified, this.#txtMinified, false);
    }

    #beautify() {
        this.#format(this.#txtMinified, this.#txtBeautified, true);
    }

    #format(from, to, indent) {
        let obj;
        try {
            obj = JSON.parse(from.value);
        } catch {
            alert("Invalid JSON");
            return;
        }

        to.value = JSON.stringify(obj, undefined, indent ? 4 : 0);
    }

});