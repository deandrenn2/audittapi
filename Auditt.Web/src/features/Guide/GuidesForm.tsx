export const GuidesForm = () => {
    return (
        <div className="w-full">
            <label className="block  font-medium mb-2" htmlFor="nombre">Nombre</label>
            <section className="w-full">
                <input type="text" id="nombre" className="w-full h-12 p-3 border rounded mb-4" />
            </section>
            <button className="bg-indigo-900 hover:bg-indigo-800 text-white font-semibold px-6 py-2 rounded">Crear</button>
        </div>
    )
}