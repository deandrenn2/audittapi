export const PatientsCreate = () => {
    return (
        <div>
            <form className="space-y-4">
                <div>
                    <label className="block text-sm font-medium mb-1">Paciente</label>
                    <input type="text" className="w-full border border-gray-300 rounded px-3 py-2" />
                </div>
                <div>
                    <label className="block text-sm font-medium mb-1">Fecha</label>
                    <input type="text" className="w-full border border-gray-300 rounded px-3 py-2" />
                </div>
                <div>
                    <label className="block text-sm font-medium mb-1">Eps</label>
                    <input type="text" className="w-full border border-gray-300 rounded px-3 py-2" />
                </div>
                <div>
                    <button type="submit" className=" bg-indigo-500 hover:bg-indigo-900 text-white px-8 py-2 rounded-lg font-semibold">
                        Crear
                    </button>
                </div>
            </form>
        </div>
    );
}