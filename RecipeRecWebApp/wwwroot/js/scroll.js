function scrollToClass(className) {
    const element = document.querySelector(`.${className}`);
    if (element) {
        element.scrollIntoView({ behavior: "smooth" });
    }
}
