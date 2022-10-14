customElements.define("main-app", class extends HTMLElement {

    #txtMinified = this.querySelector(".txt-minified");
    #txtBeautified = this.querySelector(".txt-beautified");

    #btnBeautify = this.querySelector(".btn-beautify");
    #btnMinify = this.querySelector(".btn-minify");

    constructor() {
        super();

        this.#btnBeautify.addEventListener("click", () => void this.#beautify());
        this.#btnMinify.addEventListener("click", () => void this.#minify());
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